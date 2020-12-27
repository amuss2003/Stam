pipeline {
  agent any
  stages {
    stage("build") {
      steps {
        echo 'building'
        script {
          
        }
      }
    }
    stage("deploy") {
      steps {
        echo 'deploying' 
        script {
          kubernetesDeploy(
              configs: 'azure-testapp.yaml',
              kubeconfigId: 'KubeConfig',
              enableConfigSubstitution: true
              )           
        }
      }
    }
  }
}
