using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    public static bool Long { get; private set; }
    public static bool Fly { get; private set; }

    public static void Main()
    {
        List<Pet> pets = new List<Pet>();

        int petnum = 1;
        bool input = true;


        while (input)
        {
            Console.WriteLine("Welcome to the Pet Inventory");
            Console.WriteLine();
            Console.WriteLine($"Pet {petnum}:");
            Console.WriteLine($"Kind (Dog, Cat, Lizard, Bird): ");
            var kind = (Kind)Enum.Parse(typeof(Kind), Console.ReadLine(), true);

            Console.WriteLine("Name: ");
            var name = Console.ReadLine();

            Console.WriteLine("Gender (M/F): ");
            var gender = (Gender)Enum.Parse(typeof(Gender), Console.ReadLine().ToUpper(), true);

            Console.WriteLine("Owner: ");
            var owner = Console.ReadLine();

            Pet pet = null;

            switch (kind)
            {
                case Kind.Dog:
                    Console.WriteLine("Breed: ");
                    var breed = Console.ReadLine();
                    pet = new Dog(name, gender, breed, owner);
                    break;

                case Kind.Cat:
                    Console.WriteLine("Is Longhaired? (y/n): ");
                    var isLonghaired = Console.ReadLine().ToLower() == "y";
                    pet = new Cat(name, gender, Long, owner);
                    break;

                case Kind.Lizard:
                    Console.WriteLine("Can Fly? (y/n): ");
                    var canFly = Console.ReadLine().ToLower() == "y";
                    pet = new Lizard(name, gender, Fly, owner);
                    break;

                case Kind.Bird:
                    Console.WriteLine("Can Fly? (y/n): ");
                    var canFlyBird = Console.ReadLine().ToLower() == "y";
                    pet = new Bird(name, gender, Fly, owner);
                    break;

            }
            if (pet != null)
            {
                pets.Add(pet);
            }

            Console.WriteLine("Add another pet? (y/n): ");
            input = Console.ReadLine().ToLower() == "y";
            petnum++;
        }

        Console.WriteLine("Which type of animal would you like to list? (Dog, Cat, Lizard, Bird, or 'All'): ");
        var filter = (Kind?)Enum.Parse(typeof(Kind), Console.ReadLine(), true);

        Console.WriteLine("\nAll pets in the inventory:");
        foreach (var pet in pets)
        {
            if (filter == null || pet is Dog && filter == Kind.Dog ||
                pet is Cat && filter == Kind.Cat ||
                pet is Lizard && filter == Kind.Lizard ||
                pet is Bird && filter == Kind.Bird || filter == Kind.All)
            {
                Console.WriteLine(pet);
            }
        }
    }
}


public enum Kind
{
    Dog,
    Cat,
    Lizard,
    Bird,
    All
}

public enum Gender
{
    M,
    F
}

public abstract class Pet
{
    public string Name { get; set; }
    public Gender gender { get; set; }
    public string Owner { get; set; }

    public Pet(string Name, Gender gender, string Owner)
    {
        this.Name = Name;
        this.gender = gender;
        this.Owner = Owner;
    }

    public abstract string GetDetails();
    public override abstract string ToString();

}

public class Dog : Pet
{
    public string Breed { get; set; }
    public Dog(string Name, Gender gender, string Breed, string Owner) : base(Name, gender, Owner)
    {
        this.Breed = Breed;
    }
    public override string GetDetails()
    {
        return $"{Name} ({gender}), Owner: {Owner}, Breed: {Breed}";
    }
    public override string ToString()
    {
        return $"Dog - {GetDetails()}";
    }
}

class Cat : Pet
{
    public bool Long { get; set; }
    public Cat(string Name, Gender gender, bool Long, string Owner) : base(Name, gender, Owner)
    {
        this.Long = Long;
    }
    public override string GetDetails()
    {
        return $"{Name} (Gender: {gender}, Owner: {Owner}, Hair Type: {(Long ? "Longhaired" : "Shorthaired")})";
    }

    public override string ToString()
    {
        return $"Cat - {GetDetails()}";
    }
}

class Lizard : Pet
{
    public bool Fly { get; set; }
    public Lizard(string Name, Gender gender, bool Fly, string Owner) : base(Name, gender, Owner)
    {
        this.Fly = Fly;
    }
    public override string GetDetails()
    {
        return $"{Name} (Gender: {gender}, Owner: {Owner}, Can fly: {(Fly ? "Yes" : "No")})";
    }

    public override string ToString()
    {
        return $"Lizard - {GetDetails()}";
    }


}

class Bird : Pet
{
    public bool Fly { get; set; }
    public Bird(string Name, Gender gender, bool Fly, string Owner) : base(Name, gender, Owner)
    {
        this.Fly = Fly;
    }
    public override string GetDetails()
    {
        return $"{Name} (Gender: {gender}, Owner: {Owner}, Can fly: {(Fly ? "Yes" : "No")})";
    }

    public override string ToString()
    {
        return $"Bird - {GetDetails()}";
    }
}
