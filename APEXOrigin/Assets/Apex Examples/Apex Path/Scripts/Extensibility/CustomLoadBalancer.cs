namespace Apex.Examples.Extensibility
{
    using Apex.LoadBalancing;

    /// <summary>
    /// Exposes a number of <see cref="LoadBalancedQueue" />s to balance the work load of the game.
    /// </summary>
    public class CustomLoadBalancer : LoadBalancer
    {
        /// <summary>
        /// The additional load balancer
        /// </summary>
        public static readonly ILoadBalancer extraLoadBalancer = new LoadBalancedQueue(4);

        /// <summary>
        /// Prevents a default instance of the <see cref="CustomLoadBalancer"/> class from being created.
        /// </summary>
        private CustomLoadBalancer()
        {
        }
    }
}
