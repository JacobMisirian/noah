#!/bin/bash

xbuild src/Noah.sln
sudo cp src/Noah/bin/Debug/Noah.exe /usr/bin/Noah.exe
sudo cp noah.sh /usr/bin/noah
sudo chmod 666 /usr/bin/noah
sudo chmod +x /usr/bin/noah
