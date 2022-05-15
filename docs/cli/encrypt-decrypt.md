# Encrypt and decrypt

## List ciphers
You can list available ciphers using the `list ciphers` command:
```
A1Z26Cipher
AffineCipher
AsciiXorCipher
AtbashCipher
BaconCipher
Base64Encoder
...
```

See article for all assets: [List assets](list.md)

## Encrypt and decrypt
You can encrypt using the `encrypt` command:
```sh
encrypt <cipher> <text>
encrypt <cipher> -k <key> <text>
```
```sh
decrypt <cipher> <text>
decrypt <cipher> -k <key> <text>
```

Example:
```sh
encrypt morse "hello world"
encrypt shift -k 13 "hello world"
```
```sh
decrypt morse ".... . .-.. .-.. ---  .-- --- .-. .-.. -.."
decrypt shift -k 13 "uryyb jbeyq"
```

By default, the output is not detailed. If you would like to display detailed output, add the `--detailed` flag:
```
Cipher: ShiftCipher
Key: 13
Ciphertext: uryyb jbeyq
HEX: 7572797962206A62657971
```

Display documentation and help:
```sh
encrypt --help
```
```sh
decrypt --help
```