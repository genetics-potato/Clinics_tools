require(deSolve)

p     <- 3e-2
cc    <- 2
beta  <- 8.8e-6
delta <- 2.6

yini  <- c(V=1.4e-2, T=4e8, I=0)

model <- function(t,y,params) {
	with(as.list(y), {
	
		dT <- -beta*T*V
		dI <-  beta*T*V-delta*I
		dV <-     p*I     -cc*V
		
		list(c(dT,dI,dV))
	})
}

times <- seq(from=0,to=7,by=0.001)
out   <- ode(y=yini,times=times,func=model,parms=NULL)

write.csv(out, "./Kinetics_of_influenza_A_virus_infection_in_humans.R.csv")
plot(out)