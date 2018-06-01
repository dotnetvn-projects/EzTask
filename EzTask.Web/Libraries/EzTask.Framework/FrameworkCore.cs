using AutoMapper;

public class FrameworkCore
{
    public static void SetAutoMapper(IMapper mapper)
    {
        Mapper = mapper;
    }

    internal static IMapper Mapper { get; private set; }
}
