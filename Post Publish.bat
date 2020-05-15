echo off
title Sermone Post Publish Script
cd "Sermone\bin\Release\Publish"
del /q "wwwroot\_framework\_bin\WebAssembly.Bindings.*"
findstr /V "WebAssembly.Bindings" "wwwroot\_framework\blazor.boot.json">"wwwroot\_framework\blazor.boot.json.new"
del /q "wwwroot\_framework\blazor.boot.json"
ren "wwwroot\_framework\blazor.boot.json.new" "blazor.boot.json"
..\..\..\..\Dependencies\BlazorSiginTool\BlazorSiginTool.exe "%CD%\wwwroot"
echo Finished!