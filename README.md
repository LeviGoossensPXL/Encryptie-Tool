# Encryptietool

A web-based cryptography tool built with ASP.NET Core MVC. It provides a practical interface for symmetric and
asymmetric encryption, hashing, and cryptographic key generation — useful for learning, testing, and experimenting
with common cryptographic operations.

---

## Features

### 1. Key Generation

Generate cryptographic keys ready for use in AES or RSA operations.

**AES key generation**
- Supported key sizes: 128, 192, 256 bits
- Output includes: Key and IV in both Base64 and hex format
- Downloads available: key only, IV only, or combined key+IV file

**RSA key pair generation**
- Supported key sizes: 1024, 2048, 4096 bits
- Output: public key and private key in PEM format
- Downloads available: public key (`.pem`), private key (`.pem`)

---

### 2. AES Encryption & Decryption

Symmetric encryption using the AES (Advanced Encryption Standard) algorithm.

**Encryption**
- Input: plain text or an uploaded file
- Output: Base64-encoded ciphertext, or a downloadable `.encrypted` file
- Configurable cipher mode: CBC, ECB, GCM (*disabled*)
- Configurable padding mode: PKCS7, Zeros, None
- Key and IV must be supplied as Base64 strings

**Decryption**
- Input: Base64 ciphertext or an uploaded encrypted file
- Output: recovered plain text, or a downloadable `.decrypted` file
- Same cipher mode and padding configuration as encryption

---

### 3. RSA Key Encryption / Decryption

Asymmetric encryption used to protect AES keys (hybrid encryption).

**Encrypt AES key**
- Takes a Base64-encoded AES key and an RSA public key (PEM format)
- Returns the encrypted key as Base64
- Uses OAEP padding with SHA-256

**Decrypt AES key**
- Takes a Base64-encoded encrypted AES key and an RSA private key (PEM format)
- Returns the recovered AES key as Base64
- Uses OAEP padding with SHA-256

The RSA service also supports digital signing and signature verification (SHA-256 / PKCS1), though this is not yet
exposed in the UI.

---

### 4. Hashing & Integrity Verification

Compute and verify cryptographic hashes, with support for text and file inputs.

**Supported hash algorithms**
| Algorithm | Notes |
|-----------|-------|
| MD5 | Deprecated — flagged with a warning |
| SHA-1 | Considered weak — flagged with a warning |
| SHA-256 | Recommended |
| SHA-384 | |
| SHA-512 | |

**Hash operations**
- Generate hash: produces a hex-encoded digest from text or a file
- Verify hash: compares a computed hash against an expected value (case-insensitive, ignores spaces and hyphens)

**HMAC (Hash-based Message Authentication Code)**

Provides both integrity and authenticity using a shared secret key.

- Supported algorithms: HMAC-SHA256, HMAC-SHA384, HMAC-SHA512
- Input: text or a file, plus a secret key
- Operations: generate HMAC, verify HMAC

---

## Tech Stack

- **Framework**: ASP.NET Core MVC (.NET)
- **UI**: Razor Views, Bootstrap 5
- **Cryptography**: .NET `System.Security.Cryptography` (built-in)

## Project Structure

```
WebApplication1/
├── Controllers/
│   ├── AesController.cs          # AES encrypt/decrypt + file download
│   ├── RsaController.cs          # RSA key encrypt/decrypt
│   ├── HashingController.cs      # Hash & HMAC generation/verification
│   └── KeyGenerationController.cs # AES and RSA key generation
├── Services/
│   ├── AesEncryptionService.cs   # Core AES logic
│   ├── AesKeyGenerator.cs        # AES key/IV generation
│   ├── RsaEncryptionService.cs   # Core RSA logic
│   ├── RsaKeyGenerator.cs        # RSA key pair generation
│   ├── HashingService.cs         # Hashing and HMAC logic
│   └── FileService.cs            # File upload/download handling
├── Models/                       # ViewModels
│   └── Results/                  # Result classes
└── Views/                        # Razor views per feature
    ├── Aes/
    ├── Rsa/
    ├── Hashing/
    └── KeyGeneration/
```