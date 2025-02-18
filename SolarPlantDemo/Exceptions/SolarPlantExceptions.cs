namespace SolarPlantDemo.Exceptions;

public class PlantNotFoundException(string message) : SolarPlantCustomException(message);
public class RecordGranularityException(string message) : SolarPlantCustomException(message);

