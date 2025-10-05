# sim756.HOTP
RFC 4226 (HOTP) - HMAC-Based One-Time Password Generator.

> ## NOTE - CRITICAL CHANGE!
> This library is not compatible with **TOTP (Time-based One-Time Password)** anymore! Please check the [**ISSUE #1 (FIXED)**](https://github.com/sim756/sim756.HOTP/issues/1) - https://github.com/sim756/sim756.HOTP/issues/1

> #### For **TOTP**, pelase use the following TOTP library
> NuGet - [sim756.TOTP](https://www.nuget.org/packages/sim756.TOTP) - https://www.nuget.org/packages/sim756.TOTP <br>
> GitHub - [sim756.TOTP](https://www.github.com/sim756/sim756.TOTP) - https://www.github.com/sim756/sim756.TOTP

### NuGet 
[https://www.nuget.org/packages/sim756.HOTP](https://www.nuget.org/packages/sim756.HOTP)

### GitHub
[https://github.com/sim756/sim756.HOTP](https://github.com/sim756/sim756.HOTP)

```c#
// Example 1: Using KeyString property and default counter (0)
var generator = new HOTPGenerator { KeyString = "QWERTYUIOPASDFGH" };
string hotp = generator.Compute(0);
```

```c#
// Example 2: Using KeyString property and a specific counter (e.g., 2)
var generator2 = new HOTPGenerator { KeyString = "QWERTYUIOPASDFGH" };
string nextHotp = generator2.Compute(2);
```

```c#
// Example 3: Using object initializer for KeyString and Length, and default counter (0)
var generator3 = new HOTPGenerator
{
    KeyString = "QWERTYUIOPASDFGH",
    Length = 6
};
string hotp3 = generator3.Compute(0);
```