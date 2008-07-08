#!/bin/bash

if [ ! -d "docs" ]
then
    mkdir "docs"
fi

if [ ! -d "bin" ]
then
    mkdir "bin"
fi

echo Compiling code...
gmcs ./src/*.cs ./src/Users/*.cs ./src/Misc/*.cs ./src/Channels/*.cs ./src/DCC/*.cs ./src/ServerInfo/*.cs -target:library -out:./bin/IRC.dll -keyfile:./src/IRC.key -doc:./docs/IRC.xml > /dev/null

echo Generating monodoc documentation...
monodocer -name:"IRC" -importslashdoc:./docs/IRC.xml -path:./docs/en -assembly:./bin/IRC.dll -pretty > /dev/null

echo Generating html documentation...
monodocs2html --source ./docs/en --dest ./docs/html > /dev/null

cd docs

echo Generating monodoc assembly...
mdassembler --ecma en -o IRC-docs &> /dev/null

echo "<?xml version=\"1.0\"?>" > IRC-docs.source
echo "<monodoc>" >> IRC-docs.source
echo "    <source provider=\"ecma\" basefile=\"IRC-docs\" path=\"classlib-IRC\"/>" >> IRC-docs.source
echo "</monodoc>" >> IRC-docs.source

rm -R en

cd ../

echo
echo All tasks complete
echo
echo ./bin contains binaries
echo ./docs contains documentation
echo
echo Have fun!
