using BloonsProject;
using System.CodeDom;

public class User : Player{
    public int UserID {get; set;}
    public string Username {get; set;}
    public string Password {get; set;}

    public PasswordMemento SavePasswordToMemento()
    {
        return new PasswordMemento(Password);
    }

    public void RestorePasswordFromMemento(PasswordMemento memento)
    {
        Password = memento.GetPassword();
    }

    

}

public class PasswordMemento
{
    private string Password { get; }

    public PasswordMemento(string password)
    {
        Password = password;
    }

    public string GetPassword()
    {
        return Password;
    }
}