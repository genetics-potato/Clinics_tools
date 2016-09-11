## 1. Ebola virus infection modeling and identifiability problems

+ **DOI** [10.3389/fmicb.2015.00257](https://www.doi.org/10.3389/fmicb.2015.00257)
+ **ISSN** 1664-302X

###### ABSTRACT
The recent outbreaks of Ebola virus (EBOV) infections have underlined the impact of the virus as a major threat for human health. Due to the high biosafety classification of EBOV (level 4), basic research is very limited. Therefore, the development of new avenues of thinking to advance quantitative comprehension of the virus and its interaction with the host cells is urgently needed to tackle this lethal disease.

Mathematical modelling of the EBOV dynamics can be instrumental to interpret Ebola infection kinetics on quantitative grounds. To the best of our knowledge, a mathematical modelling approach to unravel the interaction between EBOV and the host cells is still missing. In this paper, a mathematical model based on differential equations is used to represent the basic interactions between EBOV and wild-type Vero cells in vitro. Parameter sets that represent infectivity of pathogens are estimated for EBOV infection and compared with influenza virus infection kinetics. The average infecting time of wild-type Vero cells in EBOV is slower than in influenza infection. Simulation results suggest that the slow infecting time of EBOV could be compensated by its efficient replication. This study reveals several identifiability problems and what kind of experiments are necessary to advance the quantification of EBOV infection. A first mathematical approach of EBOV dynamics and the estimation of standard parameters in viral infections kinetics is the key contribution of this work, paving the way for future modelling work on EBOV infection.

![](./images/Ebola_virus_molecular_structure.png)
> **FIGURE1 | Ebola virus molecular structure.** The Ebola genomeis composed of 3 leader, nucleoprotein(NP), virion protein 35(VP35),VP40,glycoprotein(GP),VP30,VP24,polymerase(L) protein and 5 trailer(adapted from SIB SWISS Institute of Bioinformatics,2014).

![](./images/Schematic_representation_of_the model_for_EBOV_infection.png)
> **Figure 2. Schematic representation of the model for EBOV infection.** Target cells (U) are replenished with rate λ and die with rate ρ. Virus (V) infects target cells (U) with rate β. Infected cells are cleared with rate δ. Once cells are productively infected (I), they release virus at rate p and virus particles are cleared with rate c.

The EBOV infection model is considered as follows:

```vbnet
dU/dt = λ − ρU − βUV    (1)
dI/dt =     βUV − δI    (2)
dV/dt =     pI − cV     (3)
```

###### Parameters

+ The parameter ``ρ`` is fixed from literature as 0.001day<sup>−1</sup> (Moehleretal.,2005). The effect of fixing this value on the model output is evaluated with a sensitivity analysis.
+ **λ = U<sub>0</sub>ρ**
+ There is experimental evidence that the half-life of epithelium cells in lung is 17–18 months in average(Rawlins and Hogan,2008). In view of this, the infected cell death rate(δ) is fixed at 10<sup>−3</sup>.
+ **TABLE 1 | Bootstrap estimates of infection parameters<sup>*</sup>.**

|Parameters (units)|Best fit<sup>**</sup>                       |Bootstrap estimates|2.5% quantile|Median|97.5% quantile|
|------------------|------------------------------------------------------|---------|-------------|------|--------------|
|β                 |[day<sup>−1</sup>(ffu/ml)<sup>−1</sup>10<sup>−7</sup>]|1.91     |1.78         |4.06  |261.95        |
|p                 |(ffu/ml day<sup>−1</sup>cell<sup>−1</sup>)            |378      |31.80        |62.91 |580.69        |
|c                 |(day<sup>−1</sup>)                                    |8.02     |0.18         |1.05  |18.76         |
|t<sub>inf</sub>   |(hours)                                               |5.64     |1.68         |9.49  |10.7          |

\*Note that these parameter should be interpreted with the discussed identifiability problems.<br/>
\*\*Values obtained from optimization procedure to the low MOI viral titer presentedin Halfmann et al. (2008).

###### Initial Value

+ The initial number of susceptible cells(**U<sub>0</sub>**) can be taken from the experiment in Halfmann et al. (2008) as 5 × 10<sup>5</sup>.
+ The initial value for infected cells(**I<sub>0</sub>**) is set to zero. The viral titer in Halfmann et al.(2008) is measured in foci forming units per milliliter(ffu/ml).
+ The initial viral load(**V<sub>0</sub>**) is estimated from the data using the fractional polynomial model of second order(Royston and Altman, 1994). The best model based on the *Akaike Information Criterion(``AIC``)* is presented in Figure3,  providing an estimate of 9 ffu/ml for **V<sub>0</sub>**.

## 2. Identifiability Challenges in Mathematical Models of Viral Infectious Diseases

+ **ISSN** 2405-8963
+ **DOI** [10.1016/j.ifacol.2015.12.135](http://dx.doi.org/10.1016/j.ifacol.2015.12.135)

###### Abstract
Nowadays, infections by viral pathogens are one of the biggest health threats to mankind. The development of new avenues of thinking to integrate the complexity of infectious diseases and the immune system is urgently needed. Recently mathematical modelling has emerged as a tool to interpret experimental results on quantitative grounds providing relevant insights to understand several infectious diseases. Nevertheless, modelling the complex mechanisms between viruses and the immune system can result in models with a large number of parameters to be estimated. Furthermore, experimental measurements have the problem to be sparse (in time) and highly noisy. Therefore, structural and practical identifiability are key obstacles to overcome towards mathematical models with predictive value. This paper addresses the identifiability limitations in the most common mathematical model to represent viral infections. Additionally, numerical simulations reveal how initial conditions of differential equations and fixing parameter values can alter the profile likelihood.

**Keywords:** parameter estimation; identifiability; viral infections

![](./images/Viral_infection_model.png)
> **Fig. 1. Viral infection model.** Host cells can be either susceptible(``U``) or infected (``I``). Virus (``V``) infects susceptible cells with constant rate. Once cells are infected, they release virus at rate p and virus particles are cleared with rate ``c``. Infected cells can die with rate either by cytopathic viral effects or by the immune response.
>
```
U' =    - UV     (1)
I' = UV − I      (2)
V' = pI − cV     (3)
```

###### Parameters

![](./images/Best_parameter_fitting.png)
> **Fig.2. Best parameter fitting.** Cell dynamics are shown in panel(a). Viral titer data from Toapanta and Ross[2009] and simulation results are shown in panel(b). Best fitting values are β=7.22×10<sup>−7</sup>, p=9.95, c=7.12 and δ=3.07. Experiments showed that the viral titer was not detectable at day 9 for 6 mice(detection levels,less than 50 PFU/ml, are shown with a horizontal dashed line in panel(b)).

###### Initial Value
+ For initial conditions in our biological problem, the estimated number of epithelial cells(**U<sub>0</sub>**) can be experimentally fixed, which is considered as 10<sup>7</sup> cells reported by Toapanta and Ross[2009].
+ Initial values for infected cells(**I<sub>0</sub>**) are taken as zero.
+ The initial viral titre **V<sub>0</sub>** in the majority of the works is constrained to be below the detection limit(less than 50 ffu/ml). Previous modelling works suggests using half of the detection limits or less[Thi´ebautetal.,2006].