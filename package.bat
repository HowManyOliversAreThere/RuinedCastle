@REM Assemble a zip file of the files required for the mod

@REM Run release build
dotnet build -c Release

@REM Remove existing package directory
rmdir /s /q package

@REM Create new package directory
mkdir package

@REM Copy the following from root: everest.yaml, Dialog folder, Maps folder, Graphics folder
copy everest.yaml package
xcopy Dialog package\Dialog /S /E /I
xcopy Maps package\Maps /S /E /I
xcopy Graphics package\Graphics /S /E /I

@REM Copy contents of bin\Release\net7.0 to package
xcopy bin\Release\net7.0 package /S /E /I

@REM Archive the contents of the package directory as RuinedCastle.zip, not including the package folder itself
cd package
7z a -tzip ..\RuinedCastle.zip *
cd ..
