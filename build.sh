#!/usr/bin/env bash
dotnet restore -f netstandard1.6 src/**/project.json && dotnet build -f netstandard1.6 src/**/project.json