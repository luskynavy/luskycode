# ReceiptsQt

Project created with QT 6.5.3. MinGW 11.2.0 64 bit and QT Creator 16.0 with MySql driver.

## Install MySql drivers

Get `qsqlmysql.dll_Qt_SQL_driver_6.5.3_MinGW_11.2.0_64-bit.zip` from https://github.com/thecodemonkey86/qt_mysql_driver/releases/tag/qmysql_6.5.3.

Put `qsqlmysql.dll` in `D:\Qt\6.5.3\mingw_64\plugins\sqldrivers\`. So MySql will be available as drivers.

Deploy `libmysql.dll`, `libcrypto-3-x64.dll` and `libssl-3-x64.dll` to executable repertory for each configuration.

By example add a "copy file" "build steps" :
- source : %{sourceDir}\libmysql.dll
- target : %{ActiveProject:RunConfig:Executable:NativePath}\libmysql.dll.
