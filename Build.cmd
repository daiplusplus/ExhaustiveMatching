msbuild ExhaustiveMatch.sln /t:Restore

msbuild ExhaustiveMatch.sln /t:Build /p:Configuration=Release
