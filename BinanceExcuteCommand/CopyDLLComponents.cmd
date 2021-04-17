REM params
SET componentpath=C:\SourceCode\AutoTrade\BinanceExcuteCommand\Components
SET corepath=C:\Users\MRledon\Desktop\workplace\ServiceStack\src
SET TextDLLPath=C:\Users\MRledon\Desktop\workplace\ServiceStack.Text\src\ServiceStack.Text
SET RedisDllPath=C:\Users\MRledon\Desktop\workplace\ServiceStack.Redis\src\ServiceStack.Redis
SET mode=Debug
SET dotnet72=net472
SET dotnet450=net45

REM Service Stack main
copy %corepath%\ServiceStack\bin\%mode%\%dotnet72%\ServiceStack.dll %componentpath%
copy %corepath%\ServiceStack.Client\bin\%mode%\%dotnet450%\ServiceStack.Client.dll %componentpath%
copy %corepath%\ServiceStack.Common\bin\%mode%\%dotnet72%\ServiceStack.Common.dll %componentpath%
copy %corepath%\\ServiceStack.Interfaces\bin\%mode%\%dotnet72%\\ServiceStack.Interfaces.dll %componentpath%
copy %corepath%\ServiceStack.Api.OpenApi\bin\%mode%\%dotnet450%\ServiceStack.Api.OpenApi.dll %componentpath%
copy %corepath%\ServiceStack.HttpClient\bin\%mode%\%dotnet450%\ServiceStack.HttpClient.dll %componentpath%
copy %corepath%\ServiceStack.Mvc\bin\%mode%\%dotnet72%\ServiceStack.Mvc.dll %componentpath%
copy %corepath%\ServiceStack.Server\bin\%mode%\%dotnet450%\ServiceStack.Server.dll %componentpath%

REM Service Stack Text
copy %TextDLLPath%\bin\%mode%\%dotnet450%\ServiceStack.Text.dll %componentpath%
copy %TextDLLPath%\bin\%mode%\%dotnet450%\System.Memory.dll %componentpath%
copy %RedisDllPath%\bin\%mode%\%dotnet450%\ServiceStack.Redis.dll %componentpath%

pause