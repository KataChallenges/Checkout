namespace Checkout.Kata.Infrastructure.Services
{
    public interface ICheckOutSystem
    {
        int GetTotalPrice();
        void Scan(string s);
    }
}