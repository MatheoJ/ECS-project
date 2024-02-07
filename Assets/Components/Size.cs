public class Size : IComponent
{
   public float size;

    public Size(float size)
    {
        this.size = size;
    }

    public Size(Size size)
    {
        this.size = size.size;
    }
}
