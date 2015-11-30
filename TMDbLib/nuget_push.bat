for %%f in (*.nupkg) do (
    echo %%~nf
	nuget push "%%f"
	del "%%f"
)