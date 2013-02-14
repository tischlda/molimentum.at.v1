
ALTER DATABASE [$(DatabaseName)]
ADD LOG FILE 
(
    NAME = [PrimaryLogFileName]
    , FILENAME = N'$(DefaultDataPath)$(DatabaseName)_log.ldf'
    , SIZE = 3MB
    , MAXSIZE = 3MB
    , FILEGROWTH = 1MB
    
) 

