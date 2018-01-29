[Deprecated; New home : UtiLib.Delegates ( https://github.com/J-kit/UtiLib)]


# DelegateConvert

Who doesn't know the struggle: You are trying to call an generic Action or Func without knowing their exact Type. <br>
One could surely use Delegate.Method.DynamicInvoke, but that would mean a decrease in performance.

This Lightweight Libray allows you to convert any  ```Func<T1,T2,T3,TX..>``` to a nice and handy ```Func<object[], object>``` or ```Action<T1,T2,T3,TX..>``` to ```Action<object[]>```

Do not use this library when you know T at compile time, do something like that instead: 
```csharp
	Action<string, string, int> sourceAction = (x, y, z) => Console.WriteLine($"STR1: {x} STR2: {y} INT: {z}");
 	Action<object,object,object> wrapper = (x, y, z) => sourceAction((string)x,(string)y,(int)z);
```


### Examples:
#### Converting ```Action<string, string, int> ``` to ```Action<object[]>```
```csharp
	Action<string, string, int> sourceAction = (x, y, z) => Console.WriteLine($"STR1: {x} STR2: {y} INT: {z}");
	Action<object[]>  myDele = ActionConvert.Convert(sourceAction);
	myDele(new object[] { "Hey", "Yo", 123 });
```
#### Converting ```Func<int, int, int, bool> ``` to ```Func<object[], bool>```
```csharp
    Func<int, int, int, bool> checksum = (x, y, z) => x + y == z;
    Func<object[], bool> myDele = FuncConvert.Convert<bool>(checksum);
    if (myDele(new object[] { 1, 2, 3 }))
    {
    	Console.WriteLine("Yay");
    }
    else
    {
    	Console.WriteLine("Nay");
    }
```

#### Converting ```Func<int, int, int, bool> ``` to ```Func<object[], object>```
```csharp
    Func<int, int, int, bool> checksum = (x, y, z) => x + y == z;
    Func<object[], object> myDele = FuncConvert.Convert(checksum);
    if (myDele(new object[] { 1, 2, 3 }))
    {
    	Console.WriteLine("Yay");
    }
    else
    {
    	Console.WriteLine("Nay");
    }
```



If you have a feature Request, please feel free to open an issue or commit your changes. Only changes with medium or higher quality will be accepted.
