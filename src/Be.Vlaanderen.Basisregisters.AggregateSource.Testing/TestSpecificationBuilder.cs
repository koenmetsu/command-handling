namespace Be.Vlaanderen.Basisregisters.AggregateSource.Testing
{
    using System;
    using System.Linq;

    internal class TestSpecificationBuilder :
        IScenarioGivenStateBuilder,
        IScenarioGivenNoneStateBuilder,
        IScenarioWhenStateBuilder,
        IScenarioThenStateBuilder,
        IScenarioThenNoneStateBuilder,
        IScenarioThrowStateBuilder
    {
        private readonly TestSpecificationBuilderContext _context;

        public TestSpecificationBuilder() => _context = new TestSpecificationBuilderContext();

        private TestSpecificationBuilder(TestSpecificationBuilderContext context) => _context = context;

        public IScenarioGivenStateBuilder Given(params Fact[] facts)
        {
            if (facts == null)
                throw new ArgumentNullException(nameof(facts));

            return new TestSpecificationBuilder(_context.AppendGivens(facts));
        }

        public IScenarioGivenStateBuilder Given(string identifier, params object[] events)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));

            if (events == null)
                throw new ArgumentNullException(nameof(events));

            return new TestSpecificationBuilder(_context.AppendGivens(events.Select(@event => new Fact(identifier, @event))));
        }

        public IScenarioGivenNoneStateBuilder GivenNone()
            => new TestSpecificationBuilder(_context);

        public IScenarioWhenStateBuilder When(object message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            return new TestSpecificationBuilder(_context.SetWhen(message));
        }

        public IScenarioThenStateBuilder Then(params Fact[] facts)
        {
            if (facts == null)
                throw new ArgumentNullException(nameof(facts));

            return new TestSpecificationBuilder(_context.AppendThens(facts));
        }

        public IScenarioThenStateBuilder Then(string identifier, params object[] events)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));

            if (events == null)
                throw new ArgumentNullException(nameof(events));

            return new TestSpecificationBuilder(_context.AppendThens(events.Select(@event => new Fact(identifier, @event))));
        }

        public IScenarioThenNoneStateBuilder ThenNone()
            => new TestSpecificationBuilder(_context);

        public IScenarioThrowStateBuilder Throws(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            return new TestSpecificationBuilder(_context.SetThrows(exception));
        }

        EventCentricTestSpecification IEventCentricTestSpecificationBuilder.Build()
            => _context.ToEventCentricSpecification();

        ExceptionCentricTestSpecification IExceptionCentricTestSpecificationBuilder.Build()
            => _context.ToExceptionCentricSpecification();
    }
}
