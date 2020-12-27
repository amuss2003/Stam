pipeline {
  agent any
  stages {
    stage("build") {
      steps {
        echo 'building'
        script {
          withCredentials([usernamePassword(credentialsId: 'ACR', usernameVariable: 'ACR_USER', passwordVariable: 'ACR_PASSWORD')]) {
            sh 'docker login -u $ACR_USER -p $ACR_PASSWORD https://amircontainerregistry.azurecr.io'
            // build image
            def image = docker.build "amircontainerregistry.azurecr.io/samples/testci"
            // push image
            image.push()
          }
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
