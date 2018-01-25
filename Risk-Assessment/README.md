# 生存分析的Cox回归模型(比例风险模型)

> http://www.sohu.com/a/205899780_826434

在Cox比例风险模型（考克斯，1972年）基本上是常用的统计在医学研究调查的患者和一个或多个预测变量的存活时间之间的关联回归模型。

在本文中，我们将描述Cox回归模型，并提供使用R软件的实例。

需要进行多元统计建模

在临床研究中，有许多情况下，几个已知量（称为协变量）可能影响患者。

统计模型是一个经常使用的工具，可以同时分析多个因素的生存情况。另外，统计模型提供了每个因素的效应大小。

Cox比例风险模型的基础

该模型的目的是同时评估几个因素对生存的影响。换句话说，它使我们能够检查特定因素在特定时间点如何影响特定事件（例如，感染，死亡）的发生率。这个速度通常被称为危险率。预测变量（或因子）在生存分析文献中通常被称为协变量。

Cox模型由h（t）表示的危险函数表示。简而言之，危险函数可以解释为在时间t死亡的风险。可以估计如下：

h（t）=h0（t）×e×p（b1X1+b2X2+。。。+bpXp）H（Ť）=H0（Ť）×ËXp（b1X1+b2X2+。。。+bpXp）

换言之，高于1的风险比指示与事件概率正相关的协变量，并因此与生存期的长短负相关。

综上所述，

HR = 1：没有效果

HR<1：减少危险

HR> 1：危害增加

请注意，在癌症研究中：

危险比> 1（即：b> 0）的协变量被称为不良预后因素

危险比<1（即：b <0）的协变量被称为良好的预后因子

Cox模型的一个关键假设是，观察组（或患者）的危险曲线应该是成比例的并且不能交叉。

因此，考克斯模型是一个比例 - 危险模型：任何一组中事件的危险性是其他危险的常数倍数。这一假设意味着，如上所述，各组的危险曲线应该是成比例的，不能交叉。

换句话说，如果一个人在某个初始时间点有死亡风险，是另一个人的两倍，那么在所有的晚些时候，死亡风险仍然是两倍。

计算R中的Cox模型

安装并加载所需的R包

我们将使用两个R包：

生存计算生存分析

幸存者可视化生存分析结果

安装软件包

install.packages(c("survival","survminer"))

加载包

library("survival")library("survminer")

R函数来计算Cox模型：coxph（）

函数coxph（）[在生存包中]可用于计算R中的Cox比例风险回归模型。

简化格式如下：

coxph(formula,data,method)

公式：以生存对象为响应变量的线性模型。Survival对象是使用Surv（）函数创建的，如下所示：Surv（time，event）。

数据：包含变量的数据框

方法：用于指定如何处理关系。默认是'efron'。其他选项是“breslow”和“确切”。默认的“efron”通常比一度流行的“breslow”方法更受欢迎。“确切”的方法计算密集得多。

示例数据集

我们将在生存R软件包中使用肺癌数据。

data("lung")head(lung)

inst：机构代码

时间：以天为单位的生存时间

状态：审查状态1 =审查，2 =死亡

年龄：年龄

性别：男= 1女= 2

ph.ecog：ECOG表现评分（0 =好5 =死）

ph.karno：Karnofsky表现评分（bad = 0-好= 100）由医师评定

pat.karno：Karnofsky表现评分由患者评估

膳食：餐时消耗的卡路里

wt.loss：过去六个月的体重下降

计算Cox模型

我们将使用以下协变量进行Cox回归：年龄，性别，ph.ecog和wt.loss。

我们首先计算所有这些变量的单变量Cox分析;那么我们将使用两个变量来拟合多变量cox分析来描述这些因素如何共同影响生存。

单变量Cox回归

单变量Cox分析可以计算如下：

res.cox<-coxph(Surv(time,status)~sex,data=lung)res.cox

Call:coxph(formula = Surv(time, status) ~ sex, data = lung)coef exp(coef) se(coef) z psex -0.531 0.588 0.167 -3.18 0.0015Likelihood ratio test=10.6 on 1 df, p=0.00111n= 228, number of events= 165

Cox模型的函数summary（）产生更完整的报告：

summary(res.cox)

Call:coxph(formula = Surv(time, status) ~ sex, data = lung)n= 228, number of events= 165coef exp(coef) se(coef) z Pr(>|z|)sex -0.5310 0.5880 0.1672 -3.176 0.00149 **---Signif. codes: 0 '***' 0.001 '**' 0.01 '*' 0.05 '.' 0.1 ' ' 1exp(coef) exp(-coef) lower .95 upper .95sex 0.588 1.701 0.4237 0.816Concordance= 0.579 (se = 0.022 )Rsquare= 0.046 (max possible= 0.999 )Likelihood ratio test= 10.63 on 1 df, p=0.001111Wald test = 10.09 on 1 df, p=0.001491Score (logrank) test = 10.33 on 1 df, p=0.001312

Cox回归结果可以解释如下：

统计显着性。标记为“z”的列给出了Wald统计值。它对应于每个回归系数与其标准误差的比率（z = coef / se（coef））。wald统计评估是否beta（ββ）系数在统计上显着不同于0。从上面的输出，我们可以得出结论，变量性别具有高度统计学意义的系数。

回归系数。Cox模型结果中要注意的第二个特征是回归系数（coef）的符号。一个积极的信号意味着危险（死亡风险）较高，因此对于那些变量值较高的受试者，预后更差。变量性被编码为数字向量。1：男，2：女。Cox模型的R总结给出了第二组相对于第一组，即女性与男性的风险比（HR）。性别的β系数= -0.53表明在这些数据中，女性的死亡风险（低存活率）低于男性。

危害比例。指数系数（exp（coef）= exp（-0.53）= 0.59）也称为风险比，给出协变量的效应大小。例如，女性（性别= 2）将危害降低了0.59倍，即41％。女性与预后良好相关。

风险比的置信区间。总结结果还给出了风险比（exp（coef））的95％置信区间的上限和下限，下限95％界限= 0.4237，上限95％界限= 0.816。

全球统计学意义的模型。最后，输出为模型的总体显着性提供了三个替代测试的p值：可能性比率测试，Wald测试和得分logrank统计。这三种方法是渐近等价的。对于足够大的N，他们会得到相似的结果。对于小N来说，它们可能有所不同。似然比检验对于小样本量具有更好的表现，所以通常是优选的。

要一次性将单变量coxph函数应用于多个协变量，请输入：

covariates<-c("age","sex","ph.karno","ph.ecog","wt.loss")univ_formulas<-sapply(covariates,function(x)as.formula(paste('Surv(time, status)~',x)))univ_models<-lapply(univ_formulas,function(x){coxph(x,data=lung)})# Extract datauniv_results<-lapply(univ_models,function(x){x<-summary(x)p.value<-signif(x$wald["pvalue"],digits=2)wald.test<-signif(x$wald["test"],digits=2)beta<-signif(x$coef[1],digits=2);#coeficient betaHR<-signif(x$coef[2],digits=2);#exp(beta)HR.confint.lower<-signif(x$conf.int[,"lower .95"],2)HR.confint.upper<-signif(x$conf.int[,"upper .95"],2)HR<-paste0(HR," (",HR.confint.lower,"-",HR.confint.upper,")")res<-c(beta,HR,wald.test,p.value)names(res)<-c("beta","HR (95% CI for HR)","wald.test","p.value")return(res)#return(exp(cbind(coef(x),confint(x))))})res<-t(as.data.frame(univ_results,check.names=FALSE))as.data.frame(res)

beta HR (95% CI for HR) wald.test p.valueage 0.019 1 (1-1) 4.1 0.042sex -0.53 0.59 (0.42-0.82) 10 0.0015ph.karno -0.016 0.98 (0.97-1) 7.9 0.005ph.ecog 0.48 1.6 (1.3-2) 18 2.7e-05wt.loss 0.0013 1 (0.99-1) 0.05 0.83

上面的输出显示了与总体生存相关的每个变量的回归β系数，效应大小（作为危害比给出）和统计显着性。每个因素通过单独的单变量Cox回归评估。

从上面的输出中，

变量性别，年龄和ph.ecog具有高度统计学意义的系数，而ph.karno的系数不显着。

年龄和ph.ecog有正的beta系数，而性别有负系数。因此，年龄越大和ph.ecog越高，生存率越差，而女性（性别= 2）与更好的生存相关。

现在我们要描述这些因素如何共同影响生存。为了回答这个问题，我们将执行多元Cox回归分析。由于变量ph.karno在单变量Cox分析中不显着，我们将在多变量分析中跳过它。我们将3个因素（性别，年龄和ph.ecog）纳入多变量模型。

多变量Cox回归分析

时间常数协变量的死亡时间的Cox回归如下所示：

res.cox<-coxph(Surv(time,status)~age+sex+ph.ecog,data=lung)summary(res.cox)

Call:coxph(formula = Surv(time, status) ~ age + sex + ph.ecog, data = lung)n= 227, number of events= 164(1 observation deleted due to missingness)coef exp(coef) se(coef) z Pr(>|z|)age 0.011067 1.011128 0.009267 1.194 0.232416sex -0.552612 0.575445 0.167739 -3.294 0.000986 ***ph.ecog 0.463728 1.589991 0.113577 4.083 4.45e-05 ***---Signif. codes: 0 '***' 0.001 '**' 0.01 '*' 0.05 '.' 0.1 ' ' 1exp(coef) exp(-coef) lower .95 upper .95age 1.0111 0.9890 0.9929 1.0297sex 0.5754 1.7378 0.4142 0.7994ph.ecog 1.5900 0.6289 1.2727 1.9864Concordance= 0.637 (se = 0.026 )Rsquare= 0.126 (max possible= 0.999 )Likelihood ratio test= 30.5 on 3 df, p=1.083e-06Wald test = 29.93 on 3 df, p=1.428e-06Score (logrank) test = 30.5 on 3 df, p=1.083e-06

所有三个整体测试（可能性，Wald和得分）的p值都是显着的，表明该模型是显着的。这些测试评估了所有的beta（ββ）为0.在上面的例子中，检验统计是完全一致的，综合零假设被完全拒绝。

在多变量Cox分析中，协变量性别和ph.ecog仍然显着（p<0.05）。然而，协变量年龄并不显着（P = 0.23，这比0.05）。

性别p值为0.000986，危险比HR = exp（coef）= 0.58，表明患者性别和死亡风险降低之间有很强的关系。协变量的风险比可以解释为对风险的倍增效应。例如，保持其他协变量不变，女性（性别= 2）将危害降低0.58倍，即42％。我们的结论是，女性与良好的预后相关。

类似地，ph.ecog的p值是4.45e-05，危险比HR = 1.59，表明ph.ecog值与死亡风险增加之间的强关系。保持其他协变量不变，ph.ecog值越高，生存率越差。

相比之下，年龄的p值现在是p = 0.23。风险比HR = exp（coef）= 1.01，95％置信区间为0.99至1.03。由于HR的置信区间为1，这些结果表明，年龄在调整了ph值和患者性别后对HR的差异的贡献较小，并且仅趋向显着性。例如，保持其他协变量不变，额外的年龄会导致每日死亡危险因素为exp（beta）= 1.01或1％，这不是一个重要的贡献。

可视化估计的生存时间分布

已经将Cox模型拟合到数据中，可以在特定风险组的任何给定时间点可视化预测的存活比例。函数survfit（）估计生存比例，默认情况下为协变量的平均值。

# Plot the baseline survival functionggsurvplot(survfit(res.cox),color="#2E9FDF",ggtheme=theme_minimal())



我们可能希望展示估计的生存如何取决于感兴趣的协变量的值。

考虑到，我们要评估性别对估计生存概率的影响。在这种情况下，我们构建一个两行的新数据框，每个性别值一个;其他协变量被固定为它们的平均值（如果它们是连续变量的话）或者它们的最低水平（如果它们是离散变量的话）。对于虚拟协变量，平均值是数据集中编码1的比例。这个数据帧通过newdata参数传递给survfit（）：

# Create the new datasex_df<-with(lung,data.frame(sex=c(1,2),age=rep(mean(age,na.rm=TRUE),2),ph.ecog=c(1,1)))sex_df

sex age ph.ecog1 1 62.44737 12 2 62.44737 1

# Survival curvesfit<-survfit(res.cox,newdata=sex_df)ggsurvplot(fit,conf.int=TRUE,legend.labs=c("Sex=1","Sex=2"),ggtheme=theme_minimal())

