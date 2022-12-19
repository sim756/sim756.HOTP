# sim756.HOTP
HMAC-based one-time password generator.

[https://www.nuget.org/packages/sim756.HOTP](https://www.nuget.org/packages/sim756.HOTP)

[https://github.com/sim756/sim756.HOTP](https://github.com/sim756/sim756.HOTP)

```c#
string hotp = new HOTPGenerator().Compute("QWERTYUIOPASDFGH");
```
```c#
string nextHotp = new HOTPGenerator().Compute("QWERTYUIOPASDFGH", DateTime.UtcNow.AddSeconds(30));
```
```c#
string hotp = new HOTPGenerator()
{
  KeyString = "QWERTYUIOPASDFGH",
  Length = 6,
  Steps = 30
}.Compute();
```
