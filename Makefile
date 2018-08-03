MCS=/Library/Frameworks/Mono.framework/Versions/4.8.1/bin/mcs
CURPATH=`pwd`
SRVPATH=${CURPATH}/Server
SDKPATH=${CURPATH}/Ultima
REFS=System.Drawing.dll
NOWARNS=0618,0219,0414,1635

# Detect whether we are on Mac OS X environment or not
#ifeq ($(shell uname -s),Darwin)
  MONO=/Library/Frameworks/Mono.framework/Versions/4.8.1/bin/mono64
#else
#  MONO=mono
#endif

PHONY : default build clean run

default: run

debug: 
	${MCS} -target:library -out:${CURPATH}/Ultima.dll -r:${REFS} -nowarn:${NOWARNS} -d:DEBUG -d:MONO -d:ServUO -d:NEWTIMERS -nologo -debug -unsafe -recurse:${SDKPATH}/*.cs
	${MCS} -win32icon:${SRVPATH}/servuo.ico -r:${CURPATH}/Ultima.dll,${REFS} -nowarn:${NOWARNS} -target:exe -out:${CURPATH}/ServUO.exe -d:DEBUG -d:MONO -d:ServUO -d:NEWTIMERS -nologo -debug -unsafe -recurse:${SRVPATH}/*.cs
	sed -i.bak -e 's/<!--//g; s/-->//g' ServUO.exe.config

run: build
	${CURPATH}/ServUO.sh

build: ServUO.sh

clean:
	rm -f ServUO.sh
	rm -f ServUO.MONO.exe
	rm -f ServUO.MONO.pdb
	rm -f Ultima.dll
	rm -f Ultima.pdb
	rm -f *.bin

Ultima.dll: Ultima/*.cs
	${MCS} -target:library -out:${CURPATH}/Ultima.dll -r:${REFS} -nowarn:${NOWARNS} -d:MONO -d:ServUO -d:NEWTIMERS -nologo -optimize -unsafe -recurse:${SDKPATH}/*.cs

ServUO.exe: Ultima.dll Server/*.cs
	#${MCS} -win32icon:${SRVPATH}/servuo.ico -r:${CURPATH}/Ultima.dll,${REFS} -nowarn:${NOWARNS} -target:exe -out:${CURPATH}/ServUO.MONO.exe -d:ServUO -d:NEWTIMERS -d:MONO nologo -optimize -unsafe -recurse:${SRVPATH}/*.cs
	${MCS} -win32icon:${SRVPATH}/servuo.ico -r:${CURPATH}/Ultima.dll,${REFS} -nowarn:${NOWARNS} -target:exe -out:${CURPATH}/ServUO.exe -d:MONO -d:ServUO -d:NEWTIMERS -nologo -optimize -unsafe -recurse:${SRVPATH}/*.cs

ServUO.sh: ServUO.MONO.exe
	echo "#!/bin/sh" > ${CURPATH}/ServUO.sh
	echo "${MONO} ${CURPATH}/ServUO.MONO.exe" >> ${CURPATH}/ServUO.sh
	chmod a+x ${CURPATH}/ServUO.sh
	sed -i.bak -e 's/<!--//g; s/-->//g' ServUO.exe.config
