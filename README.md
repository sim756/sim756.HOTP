# sim756.HOTP
HMAC-based one-time password generator.

```c#
string hotp = new HOTPGenerator().Compute("QWERTYUIOPASDFGH");
```
```c#
string nextHotp = new HOTPGenerator().Compute("QWERTYUIOPASDFGH", DateTime.UtcNow.AddSeconds(30));
```
```c#
string hotp2 = new HOTPGenerator()
{
  KeyString = "QWERTYUIOPASDFGH",
  Length = 6,
  Steps = 30
}.Compute();
```
