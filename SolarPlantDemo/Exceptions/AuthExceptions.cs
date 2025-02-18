namespace SolarPlantDemo.Exceptions;

public class UnauthorizedException(string message) : SolarPlantCustomException(message);

public class UserCreationException(string message) : SolarPlantCustomException(message);

public class UserNotFoundException(string message) : SolarPlantCustomException(message);

public class UserLoginException(string message) : SolarPlantCustomException(message);