MKDIR c:\data\db1
MKDIR c:\data\db2
MKDIR c:\data\db3

C:\mongodb\bin\mongod --dbpath ../../data/db1 --port 27001 --replSet myreplica
C:\mongodb\bin\mongod --dbpath ../../data/db2 --port 27002 --replSet myreplica
C:\mongodb\bin\mongod --dbpath ../../data/db3 --port 27003 --replSet myreplica
