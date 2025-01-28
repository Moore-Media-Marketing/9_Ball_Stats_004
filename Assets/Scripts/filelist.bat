@echo off
(
  for /r %%f in (*.cs) do (
    if not "%%~nf"=="GenerateGameObjectList" (
      echo %%~nxf
    )
  )
)>..\TextFiles\filelist.txt