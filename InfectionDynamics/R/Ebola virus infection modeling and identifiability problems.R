require(diffEq)

# <summary>
# EBOV is known to replicate at an unusually high rate that
# overwhelms the protein synthesis of infected cells(Sanchez,
# 2001).Consistent with this observation, bootstrap estimates
# revealed a very high rate of viral replication, p = 62(95%CI 
# 31 - 580)(Table1).
# </summary>
p <- 62

# <summary>
# Although the scatter plot in Figure5 shows
# that the estimate of p can be decreased given a higher effective
# infection rate(beta),a replication rate of at least 31.8 ffu/ml cell-1
# day-1 is still needed to achieve a good fit of the viral replication
# kinetics in Figure3.
# </summary>
beta <- 31.8

# <summary>
# Distributions of
# the model parameters are shown in Figure 5. Bootstrap estimates
# for the viral clearance(median c = 1.05 day-1) is slightly below
# other viral infection results(Table1).
# </summary>
c <- 1.05

# <summary>
# There is experimental evidence that the half-life
# of epithelium cells in lung is 17–18 months in average(Rawlins
# and Hogan,2008).In view of this,the infected cell death rate(d)
# is fixed at 10-3.
# </summary>
d <- 10^(-3)

# <summary>
# The initial number of susceptible cells(U0) can be taken from the experiment in Half mannetal. (2008) as 5 × 10^5.
# </summary>
U0 <- 5 * (10^5)
# <summary>
# The parameter ? is fixed from liter-
# ature as 0.001 day-1 (Moehleretal.,2005).
# </summary>
rho <- 0.001

# <summary>
# Note that the condition ? = U0? should be satisfied to
# guarantee homeostasis in the absence of viral infection,such that
# only ? is a parameter to be determined.
# </summary>
lambda <- rho * U0

yini <- c(U=U0,I=0, V=9)

Lorenz <- function (t, y, parms) {
 with(as.list(y), {
    dU <- lambda - rho * U - beta * U * V
    dI <- beta * U * V - d * I
    dV <- p * I - c * V
	
    list(c(dU, dI, dV))
 })
}

times <- seq(from = 0, to = 5.6, by = 0.01) 
out   <- ode(y = yini, times = times, func = Lorenz, parms = NULL)