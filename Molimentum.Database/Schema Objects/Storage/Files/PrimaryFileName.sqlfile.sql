
ALTER DATABASE [$(DatabaseName)]
ADD FILE 
(
    NAME = [PrimaryFileName]
    , FILENAME = N'$(DefaultDataPath)$(DatabaseName).mdf'
    , SIZE = 3MB
    , MAXSIZE = 3MB
    , FILEGROWTH = 1MB
    
) TO FILEGROUP [PRIMARY]

