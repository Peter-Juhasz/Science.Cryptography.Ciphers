# Encrypt and decrypt

## Available ciphers
For the list of available ciphers, see article [List assets](list.md#list-ciphers).

## Encrypt and decrypt
You can encrypt using the `encrypt` command:
```sh
encrypt <cipher> <text>
decrypt <cipher> <text>
```

For example:
```sh
encrypt morse "hello world"
decrypt morse ".... . .-.. .-.. ---  .-- --- .-. .-.. -.."
```

### Keyed ciphers
Encryption key can be specified with the `-k` parameter:
```sh
encrypt <cipher> -k <key> <text>
decrypt <cipher> -k <key> <text>
```

For example:
```sh
decrypt shift -k 13 "uryyb jbeyq"
encrypt shift -k 13 "hello world"
```

## Options

### Alphabet
Some ciphers support a customization of the alphabet:
```sh
encrypt A1Z26 "hello world" --alphabet "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
```

### Encoding
Some ciphers support a customization of the encoding:
```sh
encrypt hex "hello world" --encoding US-ASCII
```

### Verbosity level
By default, the output simply contains the raw result of the operation. If you would like to display detailed output, add the `--detailed` flag:
```
Cipher: ShiftCipher
Key: 13
Ciphertext: uryyb jbeyq
HEX: 7572797962206A62657971
```

## Help
Display documentation and help:
```sh
encrypt --help
decrypt --help
```