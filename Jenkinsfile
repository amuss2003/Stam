pipeline {
  agent any
  stages {
    stage("build") {
      steps {
        echo 'building'
        withCredentials([usernamePassword(credentialsId: 'ACR', usernameVariable: 'ACR_USER', passwordVariable: 'ACR_PASSWORD')]) {
          sh 'docker login -u $ACR_USER -p $ACR_PASSWORD https://amircontainerregistry.azurecr.io'
          // build image
          def image = docker.build "amirimage12345"
          // push image
          image.push()
        }
      }
    }
    stage("test") {
      steps {
        echo 'testing'
      }
    }
    stage("deploy") {
      steps {
        echo 'deploying'       
      }
    }
  }
}
