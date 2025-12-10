static class Util
{
    static public Element GetElement(CardName name)
    {
        int nameNum = (int)name;

        if (nameNum >= (int)Element.Grass)
            return Element.Grass;

        if (nameNum >= (int)Element.Ice)
            return Element.Ice;

        if (nameNum >= (int)Element.Flame)
            return Element.Flame;

        return Element.None;
    }
}