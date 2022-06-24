# Solve a cryptogram

## Solve
Use the `solve` command to solve a cryptogram:
```sh
solve "uryyb jbeyq"
```

Example output:
```
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
Candidate found with cipher 'ShiftCipher` and score '1.0':
Key: 13
Plaintext: hello world
HEX (utf8): 68656C6C6F20776F726C64
```

For specifying all options, see article [Find key](find-key.md).

## Help
Display documentation and help:
```sh
solve --help
```