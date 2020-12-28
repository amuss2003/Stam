pipeline {
  agent any
  parameters {
    choice(name: 'BuildOptions', choices: ['Only on app files changes', 'Force'], description: 'Build Options')
  }
  environment {
    ACR_URL = 'amircontainerregistry.azurecr.io'
  }
  stages {
    stage("build") {
      when {
        expression {
          params.BuildOptions == 'Force' ||
          (params.BuildOptions == 'Only on app files changes' && changeset "TestApp*/**")
        }
      }
      steps {
        echo 'building'
        /*script {
          withCredentials([usernamePassword(credentialsId: 'ACR', usernameVariable: 'ACR_USER', passwordVariable: 'ACR_PASSWORD')]) {
            sh "docker login -u $ACR_USER -p $ACR_PASSWORD https://${ACR_URL}"
            def image = docker.build "${ACR_URL}/stable/restapi:${BUILD_TIMESTAMP}"
            image.push()
          }
        }*/
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
