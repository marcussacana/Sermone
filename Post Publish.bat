echo off
title Sermone Post Publish Script
cd "Sermone\bin\Release\netstandard2.1\Publish\wwwroot"
copy index.html 404.html
..\..\..\..\..\..\Dependencies\BlazorSiginTool\BlazorSiginTool.exe "%CD%"