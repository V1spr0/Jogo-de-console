using System;

abstract class Personagem
{
    public string Nome;
    public int Vida;
    public int AtaqueBase;
    public int Defesa;
    public int TurnosSangramento = 0;

    public abstract void Atacar(Personagem alvo, Random rnd);
    public abstract void EscolherAtaque(Personagem alvo, Random rnd);

    public void ReceberDano(int dano)
    {
        Vida -= dano;
        if (Vida < 0) Vida = 0;
    }

    public void AplicarSangramento()
    {
        TurnosSangramento = 3; // dura 3 turnos
    }

    public void SofrerSangramento()
    {
        if (TurnosSangramento > 0)
        {
            Vida -= 5;
            if (Vida < 0) Vida = 0;
            Console.WriteLine($"{Nome} sofre 5 de dano por sangramento! Vida atual: {Vida}");
            TurnosSangramento--;
        }
    }

    public bool EstaVivo() => Vida > 0;
}

//  Vampiro
class Vampiro : Personagem
{
    public Vampiro(string nome)
    {
        Nome = nome; Vida = 100; AtaqueBase = 15; Defesa = 5;
    }

    public override void EscolherAtaque(Personagem alvo, Random rnd)
    {
        Console.WriteLine("Escolha o ataque:");
        Console.WriteLine("1 - Mordida normal");
        Console.WriteLine("2 - Mordida vampírica (pode curar e aplicar sangramento)");

        string escolha = Console.ReadLine();
        if (escolha == "2") AtacarVampirico(alvo, rnd);
        else Atacar(alvo, rnd);
    }

    public override void Atacar(Personagem alvo, Random rnd)
    {
        int rolagem = rnd.Next(1, 21);
        int dano = AtaqueBase - alvo.Defesa;
        if (dano < 0) dano = 0;

        Console.WriteLine($"{Nome} rola o dado e tira {rolagem}!");
        Console.WriteLine($"{Nome} morde {alvo.Nome} causando {dano} de dano!");
        alvo.ReceberDano(dano);
    }

    private void AtacarVampirico(Personagem alvo, Random rnd)
    {
        int rolagem = rnd.Next(1, 21);
        int dano = AtaqueBase - alvo.Defesa;
        if (dano < 0) dano = 0;

        Console.WriteLine($"{Nome} tenta mordida vampírica... rola o dado e tira {rolagem}!");
        alvo.ReceberDano(dano);
        Console.WriteLine($"{alvo.Nome} perde {dano} de vida. Vida atual: {alvo.Vida}");

        if (rolagem >= 15)
        {
            int cura = dano / 2;
            Vida += cura;
            Console.WriteLine($"{Nome} ativa vampirismo e recupera {cura} de vida!");
            alvo.AplicarSangramento();
            Console.WriteLine($"{alvo.Nome} foi afetado por sangramento!");
        }
    }
}

//  Pirotecnomaniaco
class Pirotecnomaniaco : Personagem
{
    public Pirotecnomaniaco(string nome)
    {
        Nome = nome; Vida = 100; AtaqueBase = 25; Defesa = 3;
    }

    public override void EscolherAtaque(Personagem alvo, Random rnd)
    {
        Console.WriteLine("Escolha o ataque:");
        Console.WriteLine("1 - Ataque seguro");
        Console.WriteLine("2 - Ataque explosivo (mais dano, pode machucar você)");

        string escolha = Console.ReadLine();
        if (escolha == "2") AtacarExplosivo(alvo, rnd);
        else Atacar(alvo, rnd);
    }

    public override void Atacar(Personagem alvo, Random rnd)
    {
        int rolagem = rnd.Next(1, 21);
        int dano = (AtaqueBase / 2) - alvo.Defesa;
        if (dano < 0) dano = 0;

        Console.WriteLine($"{Nome} rola o dado e tira {rolagem}!");
        Console.WriteLine($"{Nome} faz um ataque seguro em {alvo.Nome}, causando {dano} de dano!");
        alvo.ReceberDano(dano);
    }

    private void AtacarExplosivo(Personagem alvo, Random rnd)
    {
        int rolagem = rnd.Next(1, 21);
        int dano = AtaqueBase - alvo.Defesa;
        if (dano < 0) dano = 0;

        Console.WriteLine($"{Nome} prepara ataque explosivo... rola o dado e tira {rolagem}!");

        if (rolagem <= 5)
        {
            Console.WriteLine($"{Nome} se explodiu acidentalmente!");
            this.ReceberDano(10);
            Console.WriteLine($"{Nome} perdeu 10 de vida. Vida atual: {Vida}");
        }
        else
        {
            Console.WriteLine($"{alvo.Nome} leva {dano} de dano da explosão!");
            alvo.ReceberDano(dano);
        }
    }
}

//  Mago
class Mago : Personagem
{
    public int Mana;

    public Mago(string nome)
    {
        Nome = nome; Vida = 80; AtaqueBase = 20; Defesa = 2; Mana = 50;
    }

    public override void EscolherAtaque(Personagem alvo, Random rnd)
    {
        Console.WriteLine("Escolha o ataque:");
        Console.WriteLine("1 - Magia fraca (sem custo)");
        Console.WriteLine("2 - Magia forte (10 de mana, ignora defesa)");

        string escolha = Console.ReadLine();
        if (escolha == "2") AtacarForte(alvo, rnd);
        else Atacar(alvo, rnd);
    }

    public override void Atacar(Personagem alvo, Random rnd)
    {
        int rolagem = rnd.Next(1, 21);
        Console.WriteLine($"{Nome} rola o dado e tira {rolagem}!");
        Console.WriteLine($"{Nome} lança magia fraca em {alvo.Nome}, causando 5 de dano!");
        alvo.ReceberDano(5);
    }

    private void AtacarForte(Personagem alvo, Random rnd)
    {
        int rolagem = rnd.Next(1, 21);
        Console.WriteLine($"{Nome} tenta magia poderosa... rola o dado e tira {rolagem}!");

        if (Mana >= 10)
        {
            Mana -= 10;
            Console.WriteLine($"{Nome} gasta 10 de mana e lança magia forte!");
            alvo.ReceberDano(AtaqueBase);
            Console.WriteLine($"{alvo.Nome} perde {AtaqueBase} de vida (ignora defesa). Vida atual: {alvo.Vida}");
        }
        else
        {
            Console.WriteLine($"{Nome} está sem mana! Usa magia fraca.");
            Atacar(alvo, rnd);
        }
    }
}

//  Guerreiro
class Guerreiro : Personagem
{
    public Guerreiro(string nome)
    {
        Nome = nome; Vida = 120; AtaqueBase = 18; Defesa = 8;
    }

    public override void EscolherAtaque(Personagem alvo, Random rnd)
    {
        Console.WriteLine("Escolha o ataque:");
        Console.WriteLine("1 - Ataque normal");
        Console.WriteLine("2 - Ataque com chance crítica (20%)");

        string escolha = Console.ReadLine();
        if (escolha == "2") AtacarCritico(alvo, rnd);
        else Atacar(alvo, rnd);
    }

    public override void Atacar(Personagem alvo, Random rnd)
    {
        int rolagem = rnd.Next(1, 21);
        int dano = AtaqueBase - alvo.Defesa;
        if (dano < 0) dano = 0;

        Console.WriteLine($"{Nome} rola o dado e tira {rolagem}!");
        Console.WriteLine($"{Nome} golpeia {alvo.Nome}, causando {dano} de dano!");
        alvo.ReceberDano(dano);
    }

    private void AtacarCritico(Personagem alvo, Random rnd)
    {
        int rolagem = rnd.Next(1, 21);
        int dano = AtaqueBase - alvo.Defesa;
        if (dano < 0) dano = 0;

        Console.WriteLine($"{Nome} tenta golpe crítico... rola o dado e tira {rolagem}!");

        int chance = rnd.Next(1, 101);
        if (chance <= 20)
        {
            dano *= 2;
            Console.WriteLine("💥 GOLPE CRÍTICO!");
        }

        alvo.ReceberDano(dano);
        Console.WriteLine($"{alvo.Nome} perdeu {dano} de vida. Vida atual: {alvo.Vida}");
    }
}

// Classe Batalha
class Batalha
{
    public void Lutar(Personagem p1, Personagem p2)
    {
        Random rnd = new Random();
        Console.WriteLine($"\nBATALHA INICIADA: {p1.Nome} VS {p2.Nome}!\n");

        while (p1.EstaVivo() && p2.EstaVivo())
        {
            Console.WriteLine($"\n--- Turno de {p1.Nome} ---");
            p1.EscolherAtaque(p2, rnd);
            p2.SofrerSangramento();
            if (!p2.EstaVivo()) break;

            Console.WriteLine($"\n--- Turno de {p2.Nome} ---");
            p2.EscolherAtaque(p1, rnd);
            p1.SofrerSangramento();
        }

        Console.WriteLine("\n----- RESULTADO -----");
        if (p1.EstaVivo())
            Console.WriteLine($"{p1.Nome} venceu a batalha!");
        else
            Console.WriteLine($"{p2.Nome} venceu a batalha!");
    }
}

//  Programa principal
class Program
{
    static Personagem CriarPersonagem(string jogador)
    {
        Console.WriteLine($"\n{jogador}, escolha seu personagem:");
        Console.WriteLine("1 - Vampiro");
        Console.WriteLine("2 - Pirotecnomaniaco");
        Console.WriteLine("3 - Mago");
        Console.WriteLine("4 - Guerreiro");

        string escolha = Console.ReadLine();
        switch (escolha)
        {
            case "1": return new Vampiro("Vampiro");
            case "2": return new Pirotecnomaniaco("Pirotecnomaniaco");
            case "3": return new Mago("Mago");
            case "4": return new Guerreiro("Guerreiro");
            default: return new Guerreiro("Guerreiro");
        }
    }

    static void Main()
    {
        Personagem jogador1 = CriarPersonagem("Jogador 1");
        Personagem jogador2 = CriarPersonagem("Jogador 2");

        Batalha batalha = new Batalha();
        batalha.Lutar(jogador1, jogador2);
    }
}

