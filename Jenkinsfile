pipeline {
  agent any
  stages {
    stage("build") {
      steps {
        echo 'building'
        script {
          withCredentials([usernamePassword(credentialsId: 'ACR', usernameVariable: 'ACR_USER', passwordVariable: 'ACR_PASSWORD')]) {
            sh 'docker login -u $ACR_USER -p $ACR_PASSWORD https://amircontainerregistry.azurecr.io'
            def image = docker.build "amircontainerregistry.azurecr.io/samples/testci:${BUILD_TIMESTAMP}"
            image.push()
          }
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
