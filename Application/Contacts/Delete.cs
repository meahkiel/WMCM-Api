using Application.SeedWorks;

namespace Application.Contacts;

public class Delete
{

    public class Command : IRequest<Result<Unit>>
    {
        public string Id { get; set; }
    }

    public class CommandHandler : IRequestHandler<Command,Result<Unit>>
    {
        private readonly UnitWrapper _context;

        public CommandHandler(UnitWrapper context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {

                var customer = await _context.Customers.GetSingle(Guid.Parse(request.Id));

                if (customer == null)
                    throw new NullReferenceException();

                _context.Customers.Remove(customer);
                
                await _context.SaveChangesAsync();
                
                return Result<Unit>.Success(Unit.Value);
            }
            catch (Exception ex)
            {
                return Result<Unit>.Failure(ex.Message);
            }
           
        }
    }



}
