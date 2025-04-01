# ReceiptsQt

Project created with QT 6.2.0 MinGW 8.1.0 64 bit and QT Creator 16.0 with MySql driver.

## Install MySql drivers

Get `qsqlmysql.dll_Qt_SQL_driver_6.2.0_MinGW_8.1.0_64-bit.zip` from https://github.com/thecodemonkey86/qt_mysql_driver/releases/tag/qmysql_6.2.0.

Put `qsqlmysql.dll` in `D:\Qt\6.2.0\mingw81_64\plugins\sqldrivers\`. So MySql will be available as drivers.

Deploy `libmysql.dll`, `libcrypto-1_1-x64.dll` and `libssl-1_1-x64.dll` to executable repertory for each configuration.

By example with copy build steps :
- source : %{sourceDir}\libmysql.dll
- target : %{ActiveProject:RunConfig:Executable:NativePath}\libmysql.dll.
