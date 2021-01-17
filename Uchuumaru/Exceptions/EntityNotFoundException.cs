using System;

namespace Uchuumaru.Exceptions
{
    public class EntityNotFoundException<TEntity> : Exception
    {
        public EntityNotFoundException() : base(nameof(TEntity))
        {
        }
    }
}