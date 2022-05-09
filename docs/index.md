---
title: Home
description: "Using the what3words api to get 3 random words based on a randomly generated co-ordinate location."
---

Using the [what3words](https://what3words.com) api to get 3 random words based on a randomly generated co-ordinate location.

Words are the processed to:

* Set pre text
* Set post text
* Set word separator
* Uppercase 1st letter

Optionally generated passwords can then be checked against the [pwnd password api](https://haveibeenpwned.com/Passwords) and also using the C# port of the dropbox [zxcvbn strength estimator](https://dropbox.tech/security/zxcvbn-realistic-password-strength-estimation)

## Usage

```cmd
USAGE:
    What3Passwords run [OPTIONS]

OPTIONS:
    -h, --help                Prints help information
        --api-key <KEY>
        --number <NUMBER>
        --pre <TEXT>
        --post <TEXT>
        --separator <TEXT>
        --upper-case
        --double-up
        --include-numbers
        --check-quality
        --check-pwnd
```

***NOTE*** In order to run you will need a [what3words API key](https://developer.what3words.com/public-api)
