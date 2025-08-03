# One Tool To Rule Them All

![Banner](assets/banner.)

.NET in Memory Execution

## Overview

RuleThemAll consists of two main components:

- **Encryptor**: A utility for encrypting files and parameters using XOR encryption
- **RuleThemAll**: A console application that loads and executes encrypted binaries with sandbox evasion

## Projects

### 🔐 Encryptor

A utility tool for encrypting files and command-line parameters.

**Features:**
- XOR encryption with configurable keys
- File encryption (creates .bin files)
- Parameter encryption (Base64 output)
- Support for any file type

**Build:**
```bash
cd Encryptor
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe Encryptor.csproj /p:Configuration=Release
```

**Usage Examples:**
```bash
# Encrypt a file
Encryptor.exe Rubeus.exe
# Output: Rubeus.bin

# Encrypt parameters
Encryptor.exe --param /nowrap /ptt
# Output: Base64 encrypted string
```

### ⚡ RuleThemAll

A stealth console application that executes encrypted binaries with sandbox evasion.

**Features:**
- Sandbox timeout evasion (10-second mathematical operations)
- Encrypted binary loading from embedded resources
- Encrypted parameter support
- Minimal console output for operational security
- AMSI/ETW patching via scarlet_witch.dll

**Build:**
```bash
cd RuleThemAll
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe RuleThemAll.csproj /p:Configuration=Release /p:Platform=x64
```

**Usage Examples:**
```bash
# Normal mode (interactive parameters)
RuleThemAll.exe
# Prompts: params: /nowrap /ptt

# Encrypted parameter mode
RuleThemAll.exe --param <encrypted_base64_string>
```

## Complete Workflow

### 1. Prepare Your Binary
```bash
# Encrypt your .NET binary
Encryptor.exe Rubeus.exe
# Copy Rubeus.bin to RuleThemAll/encrypted.png
```

### 2. Prepare Your Parameters
```bash
# Encrypt your parameters
Encryptor.exe --param /nowrap /ptt
# Copy the Base64 output
```

### 3. Execute
```bash
# Run with encrypted parameters
RuleThemAll.exe --param <your_encrypted_string>
```

## Architecture

```
RuleThemAll/
├── Encryptor/           # Encryption utility
│   ├── Program.cs       # XOR encryption logic
│   └── Encryptor.csproj
├── RuleThemAll/         # Main execution engine
│   ├── Bypass.cs        # Core execution logic
│   ├── encrypted.png    # Embedded binary resource
│   ├── scarlet_witch.dll # AMSI/ETW patch
│   └── RuleThemAll.csproj
├── assets/              # Images and resources
└── README.md           # This file
```

## Security Features

- **Sandbox Evasion**: Mathematical operations to bypass timeout detection
- **Encrypted Resources**: Binary payloads embedded as encrypted resources
- **Parameter Obfuscation**: Command-line parameters can be encrypted
- **Stealth Mode**: Minimal logging for operational security
- **AMSI/ETW Bypass**: Integration with scarlet_witch.dll for patch execution

## Requirements

- .NET Framework 4.8
- Windows x64
- scarlet_witch.dll (for AMSI/ETW patching)
- Encrypted binary embedded as resource

## Build All Projects

```bash
# Build Encryptor
cd Encryptor
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe Encryptor.csproj /p:Configuration=Release

# Build RuleThemAll
cd ../RuleThemAll
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe RuleThemAll.csproj /p:Configuration=Release /p:Platform=x64
```

## Execution Flow

1. **Parameter Processing**: Normal input or encrypted parameter decryption
2. **Sandbox Delay**: 10-second mathematical operations
3. **Binary Loading**: Decrypt embedded binary from resource
4. **Execution**: Run decrypted .NET binary with parameters
5. **Output**: Execute with provided parameters

## XOR Key

Both projects use the same XOR key:
```csharp
byte[] xorKey = { 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48 };
```

## License

This project is for educational and research purposes only. 