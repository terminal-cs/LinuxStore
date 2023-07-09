run:
	@dotnet publish -c Release -v 0
	@sudo cp ./bin/Release/net7.0/linux-x64/native/LinuxStore /bin/store

debug:
	@dotnet build -v 0

uninstall:
	@sudo rm /bin/linuxstore