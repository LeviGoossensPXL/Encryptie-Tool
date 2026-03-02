# Encryptie tool

## folder structure (proposed)
```
.
└── Encryptie-tool.WebApp/
    └── WebApplication1/
        ├── Entities (database entities)
        ├── Models (view models)
        ├── Repositories (repository classes)/
        │   └── Interfaces (repository interfaces)
        ├── Services (service classes)/
        │   └── Interfaces (service interfaces)
        └── Views (webpages)
```

## Strategy (proposed)
1. select the issue
![step 1](Screenshot_2026-03-02_201408.png)
2. make a branch
![step 2](Screenshot_2026-03-02_201929.png)
3. select "dev" as a source
![step 3](Screenshot_2026-03-02_202259.png)
4. switch to branch in visual studio or execute the given commands in the terminal (powershell) of visual studio.
![step 4.1](Screenshot_2026-03-02_202537.png)
![step 4.2](Screenshot_2026-03-02_202915.png)
5. write code ...

6. make pull request on github to "dev" branch

7. get it reviewed by one other person

8. reviewer approves it

9. someone merges it to "dev" branch