REM params
SET componentpath=C:\SourceCode\AutoTrade\BinanceExcuteCommand\Components
SET codePath=C:\SourceCode\SanGiaoDich\API
SET mode=Debug

REM dll
copy %codePath%\BackEndAPI.DataEntities\bin\%mode%\netcoreapp2.1\BackEndAPI.DataEntities.dll %componentpath%

REM pdb
copy %codePath%\BackEndAPI.DataEntities\bin\%mode%\netcoreapp2.1\BackEndAPI.DataEntities.pdb %componentpath%

pause