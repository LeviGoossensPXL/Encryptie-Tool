# Architecture

## Overview

Encryptietool is an ASP.NET Core 8 MVC web application. It follows a classic three-layer MVC structure with a service 
layer sitting between controllers and the cryptographic logic. All cryptographic operations use .NET's built-in 
`System.Security.Cryptography` namespace — no third-party crypto libraries.

---

## Layer Structure

```
Request
  │
  ▼
Controller          — handles HTTP, orchestrates flow, returns views
  │
  ▼
Service             — contains all business/crypto logic
  │
  ▼
Result / Model      — carries output back up to the controller
```

Controllers do not contain any cryptographic logic. They parse input, call one or more services,
and set output properties on the ViewModel before returning a view.

---

## Dependency Injection

All services are registered as **Scoped** in `Program.cs`:

| Interface | Implementation |
|---|---|
| `IAesEncryptionService` | `AesEncryptionService` |
| `IAesKeyGenerator` | `AesKeyGenerator` |
| `IRsaEncryptionService` | `RsaEncryptionService` |
| `IRsaKeyGenerator` | `RsaKeyGenerator` |
| `IHashingService` | `HashingService` |
| `IFileService` | `FileService` |

Controllers receive their dependencies via constructor injection. This makes each service independently replaceable
and testable.

---

## Result Pattern

Service methods never throw for expected failures. They return a result object that inherits from `BaseResult`:

```csharp
public abstract class BaseResult
{
    public bool Succeeded { get; set; } = true;
    public IEnumerable<string> Errors => _errors;
    public void Failed(string errorMessage) { ... }
}
```

Concrete result types extend this with operation-specific data:

| Result class | Extra fields |
|---|---|
| `AesEncryptionResult` | `CipherText` (string), `FileInfo` |
| `AesDecryptionResult` | `PlainText` (string), `FileInfo` |
| `AesKeyResult` | `KeyBase64`, `KeyHex`, `IVBase64`, `IVHex`, `KeyBytes`, `IVBytes`, `KeySize` |
| `RsaKeyResult` | `PublicKeyPem`, `PrivateKeyPem`, `KeySize` |
| `FileSaveResult` | `FileInfo` |
| `FileDownloadResult` | `FileInfo` |
| `FileDeleteResult` | _(no extra fields)_ |

Controllers check `result.Succeeded` before proceeding, keeping error handling consistent and the controller logic clean.

---

## File Handling

File operations are centralized in `FileService`. Uploaded files are never stored under their original name — `Save()`
writes each file to `wwwroot/Files/` using a `Guid` as the filename, preventing name collisions and path traversal:

```csharp
string filePath = Path.Combine(_filesFolder, Guid.NewGuid().ToString());
```

The AES service receives a `FileInfo` pointing to the saved file and writes its output as a sibling file with
a `.encrypted` or `.decrypted` suffix appended. The controller then exposes the output filename through
a `Download` action that reads and streams the file back.

```
Upload → FileService.Save() → wwwroot/Files/<guid>
                                      │
                              AesEncryptionService
                                      │
                              wwwroot/Files/<guid>.encrypted
                                      │
                              AesController.Download()
```

---

## AES Encryption Flow

`AesEncryptionService` wraps .NET's `Aes` class. A private `ConfigureAes()` method applies key, IV, cipher mode,
and padding mode before handing back an `Aes` instance, keeping the four operation methods
(`Encrypt`, `Decrypt`, `EncryptFile`, `DecryptFile`) free of repeated setup.

**Text path:** uses in-memory `MemoryStream` + `CryptoStream`, output is Base64-encoded.

**File path:** uses `FileStream` directly. The crypto transform is applied as a read-mode `CryptoStream` over the input,
which is piped to an output `FileStream`. No full-file byte array is held in memory.

Key and IV are always passed as Base64 strings from the view and decoded to `byte[]` inside `ConfigureAes()`.

---

## RSA Flow

`RsaEncryptionService` wraps .NET's `RSA` class. The controller (`RsaController`) is responsible for importing
the PEM key string into an `RSA` instance before passing it to the service. The service itself is stateless
and key-format-agnostic — it receives an `RSA` object:

```csharp
// Controller
var rsa = RSA.Create();
rsa.ImportFromPem(rsaViewModel.RsaPublicKey);
var result = _rsaEncryptionService.EncryptKey(aesKeyBytes, rsa);
```

Encryption uses `RSAEncryptionPadding.OaepSHA256`. The service also has `SignData` / `VerifySignature` methods
(PKCS1, SHA-256) but these are not yet wired to the UI.

---

## Hashing Flow

`HashingService` is fully stateless. It creates a new hash or HMAC algorithm instance per call via factory
switch expressions:

```csharp
private static HashAlgorithm CreateHashAlgorithm(HashAlgorithmType type) => type switch
{
    HashAlgorithmType.MD5    => MD5.Create(),
    HashAlgorithmType.SHA1   => SHA1.Create(),
    HashAlgorithmType.SHA256 => SHA256.Create(),
    ...
};
```

Hash verification normalizes expected hash strings before comparison (strips spaces, hyphens, trims whitespace)
and uses `OrdinalIgnoreCase` so hex strings from different tools still match.

HMAC methods follow the same structure, using a separate `CreateHmacAlgorithm()` factory.

---

## ViewModel Design

Each feature area has its own ViewModel(s). ViewModels carry both input fields (bound on POST) and output fields
(set by the controller and rendered in the same view on the response).
This means a single round-trip GET→POST→view contains both the form and its result.

`AesEncryptionViewModel` and `AesDecryptionViewModel` include an `IsFileUpload` boolean that the controller uses
to branch between text and file paths, avoiding separate action methods for each mode.

---

## Middleware Pipeline

Configured in `Program.cs`:

```
HTTPS Redirection
Static Files
Routing
Authorization
Controller Route: {controller=Home}/{action=Index}/{id?}
```

In non-development environments HSTS is enabled.

---

## Dependencies

| Package | Purpose |
|---|---|
| `Microsoft.AspNetCore.Identity.EntityFrameworkCore` 8.0.23 | Included (scaffolded), not actively used |
| `Microsoft.EntityFrameworkCore.SqlServer` 8.0.23 | Included (scaffolded), not actively used |
| `Microsoft.VisualStudio.Web.CodeGeneration.Design` 8.0.23 | Scaffolding tooling only |

All cryptographic functionality uses only .NET 8 built-in APIs — no external crypto dependencies.

---

## Algorithms Used

### AES (Symmetric Encryption)

| Property | Details |
|---|---|
| Algorithm | AES (Rijndael, 128-bit block size) |
| Key sizes | 128, 192, 256 bits |
| IV size | 128 bits (16 bytes), always required |
| Cipher modes | CBC, ECB, CFB, CTS, OFB |
| Padding modes | PKCS7, Zeros, ANSIX923, ISO10126, None |
| Key/IV encoding | Base64 strings in the UI, decoded to `byte[]` before use |
| Output encoding | Base64 (text path); raw binary file (file path) |
| .NET type | `System.Security.Cryptography.Aes` |

The cipher mode and padding mode are user-selectable, which means insecure combinations (e.g. ECB) can be chosen
intentionally for educational purposes.

---

### RSA (Asymmetric Encryption)

| Property | Details |
|---|---|
| Algorithm | RSA |
| Key sizes | 1024, 2048, 4096 bits |
| Key format | PEM (PKCS#1) — `ExportRSAPublicKeyPem` / `ExportRSAPrivateKeyPem` |
| Encryption padding | OAEP with SHA-256 |
| Signature hash | SHA-256 (service layer only, not exposed in UI) |
| Signature padding | PKCS1 (service layer only, not exposed in UI) |
| Input/output encoding | Base64 |
| .NET type | `System.Security.Cryptography.RSA` |

RSA is used in a hybrid encryption pattern: it encrypts an AES key, not arbitrary data. This is the correct use
of RSA — direct RSA encryption of large data is not supported.

---

### Hashing

| Algorithm | Output size | Status |
|---|---|---|
| MD5 | 128 bits (16 bytes) | Deprecated — UI shows warning |
| SHA-1 | 160 bits (20 bytes) | Weak — UI shows warning |
| SHA-256 | 256 bits (32 bytes) | Recommended |
| SHA-384 | 384 bits (48 bytes) | Recommended |
| SHA-512 | 512 bits (64 bytes) | Recommended |

Output is hex-encoded. All algorithms are from `System.Security.Cryptography`.

---

### HMAC (Keyed Hashing)

| Algorithm | Underlying hash | Key |
|---|---|---|
| HMACSHA256 | SHA-256 | UTF-8 encoded secret string |
| HMACSHA384 | SHA-384 | UTF-8 encoded secret string |
| HMACSHA512 | SHA-512 | UTF-8 encoded secret string |

Output is hex-encoded. HMAC secret keys are accepted as plain UTF-8 strings from the UI — not Base64 or hex.

---

## Security Considerations

### What is handled well

**OAEP padding for RSA.** The service uses `RSAEncryptionPadding.OaepSHA256` instead of the older PKCS1v1.5 padding.
PKCS1v1.5 is vulnerable to padding oracle attacks (Bleichenbacher); OAEP is the correct choice.

**CSRF protection on sensitive actions.** POST endpoints on `HashingController` and `KeyGenerationController` use
`[ValidateAntiForgeryToken]`. This prevents cross-site request forgery on hash and key generation operations.

**No original filenames stored.** `FileService` saves uploads under a random `Guid`, preventing filename-based
path traversal and avoiding collisions.

**Constant-time-equivalent hash comparison.** Hash verification uses
`string.Equals(..., StringComparison.OrdinalIgnoreCase)` after normalizing both values.
This is not a true constant-time comparison, but since hashes are not secret values being guarded against
timing attacks in this context, it is acceptable.

**MD5/SHA-1 warnings.** The UI explicitly warns users when they select deprecated or weak hash algorithms.

---

### Known weaknesses and risks

**ECB mode is selectable.** ECB (Electronic Codebook) does not use an IV and produces identical ciphertext blocks
for identical plaintext blocks, leaking data patterns. It should not be used for real encryption. Since this tool
is educational, it is intentionally available but no warning is shown in the UI.

**CSRF missing on AES controller.** `AesController` POST actions do not use `[ValidateAntiForgeryToken]`.
An attacker could trick a logged-in user into submitting an encryption or decryption form.

**Files are never cleaned up.** Uploaded and processed files accumulate in `wwwroot/Files/`.
Sensitive plaintext files uploaded for encryption remain on disk indefinitely.
Any file whose Guid name is known (or guessed) can be downloaded via the `Download` action — there is no ownership check.

**Download endpoint accepts arbitrary filenames.** `AesController.Download(string filename)` resolves
the file by joining the provided name against `wwwroot/Files/`. Although `FileService.Download()` only checks for
file existence and does not use `..` segments, the filename is user-controlled and not sanitized,
which is a path traversal risk if the logic were extended.

**RSA 1024-bit key size is offered.** 1024-bit RSA is considered below the security threshold recommended by
NIST (2048-bit minimum). It is available in the key generation UI with no warning.

**Keys and IVs are submitted in plain HTTP form fields.** If the application is served over HTTP (not HTTPS),
keys are transmitted in cleartext. HTTPS redirection is configured in `Program.cs` but is only enforced in
non-development environments.

**HMAC keys are plain UTF-8 strings.** Accepting a human-typed string as an HMAC key limits effective
entropy compared to a randomly generated key. No minimum key length is enforced.

**No authentication or authorization.** All cryptographic operations and file downloads are publicly accessible.

---

## Key Design Decisions

**No persistence layer in use.** EF Core and Identity packages are referenced but no `DbContext` or repository
implementations exist yet. Placeholder empty folders (`Entities\`, `Repositories\Interfaces\`) suggest
this was scaffolded for future use.

**File storage is local disk.** Uploaded and processed files live in `wwwroot/Files/` on the server.
There is no cleanup mechanism; files accumulate until deleted manually or via `FileService.Delete()`.

**No authentication or authorization.** All routes are publicly accessible.

**Services are stateless.** All state lives in the ViewModel per request. This means services can safely
remain Scoped without any concurrency concerns.