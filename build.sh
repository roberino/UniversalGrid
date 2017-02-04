#!/usr/bin/env bash
dotnet restore -f netstandard1.6 && dotnet build -f netstandard1.6 src/**/project.json