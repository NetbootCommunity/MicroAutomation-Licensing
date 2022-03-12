#!/bin/bash

# Get the token as the first parameter
token=$1

dotnet dotnet-sonarscanner begin \
  /k:"MicroAutomation-Licensing" \
  /n:"MicroAutomation Licensing" \
  /d:"sonar.host.url=https://sonarqube.netboot.fr" \
  /d:"sonar.sources=." \
  /d:"sonar.login=$token" \
  /d:"sonar.verbose=TRUE"

dotnet dotnet-sonarscanner end \
  /d:sonar.login=$token